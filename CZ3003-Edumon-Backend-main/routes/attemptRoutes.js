const express = require('express')
const router = express.Router()

const COMPLETION_STATUS = ['IN_PROGRESS', 'COMPLETED']
const QUESTION_GROUP = ['ASSIGNMENT', 'CHALLENGE', 'GYM']

module.exports = ({ db }) => {
    router.get('/email/:email', async (req, res) => {
        try {
            const userEmail = req.params.email
            if (userEmail == null) {
                throw ({
                    status: 'fail',
                    message: 'No user_email Given for Query'
                })
            }
            const attemptRef = db.collection('attempts')
            const attempts = await attemptRef.where('user_email', '==', userEmail).get()
            const data = []
            if (!attempts.empty) {
                attempts.forEach(doc => {
                    data.push({
                        attempt_id: doc.id,
                        ...doc.data(),
                    })
                })
            }

            res.status(200)
            res.json({
                status: 'success',
                data
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    router.get('/:id', async (req, res) => {
        const attemptId = req.params.id
        try {
            const attempt = db.collection('attempts').doc(attemptId)
            const doc = await attempt.get()
            var data
            if (doc.exists) {
                data = {
                    attempt_id: doc.id,
                    ...doc.data(),
                }    
            } else {
                data = null
            }
            
            res.status(200)
            res.json({
                status: 'success',
                data
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    router.post('/', async (req, res) => {
        try {
            const questionSet = req.body.question_set
            if (questionSet == null) {
                throw ({
                    status: 'fail',
                    message: 'No Question Set (Assignment, Challenge, Gym) Chosen'
                })
            }
            var questionGroup = req.body.question_group
            if (questionGroup == null) {
                throw ({
                    status: 'fail',
                    message: 'No Question Group (Assignment, Challenge, Gym) Given'
                })
            }
            questionGroup = questionGroup.toUpperCase()
            if (!QUESTION_GROUP.includes(questionGroup)) {
                throw ({
                    status: 'fail',
                    message: 'Question Group can Only Take 3 Different Values [ASSIGNMENT, CHALLENGE, GYM]'
                })
            }
            const userEmail = req.body.user_email
            if (userEmail == null) {
                throw ({
                    status: 'fail',
                    message: 'No User Email Given'
                })
            }
            const userAnswers = req.body.user_answers
            if (userAnswers == null) {
                throw ({
                    status: 'fail',
                    message: 'Empty Answers'
                })
            }

            // Get Question Set
            const ref = db.collection(questionGroup.toLowerCase() + 's').doc(questionSet)
            const questionSetInfo = await ref.get()
            if (!questionSetInfo.exists) {
                throw ({
                    status: 'fail',
                    message: 'Given Question Set Id Matches No Existing Set'
                })
            }

            // Check Completion Status
            const userAnswerKeys = Object.keys(userAnswers)
            const questionCount = questionSetInfo.data().question_count
            var completionStatus
            if (userAnswerKeys.length > questionCount) {
                throw ({
                    status: 'fail',
                    message: 'Number of Answers Submitted Exceeds Number of Questions'
                })
            } else if (userAnswerKeys.length < questionCount) {
                completionStatus = COMPLETION_STATUS[0]
            } else {
                completionStatus = COMPLETION_STATUS[1]
            }

            // Calculate Score
            var score = 0
            let promises = []
            userAnswerKeys.forEach(key => {
                const questionRef = db.collection('questions').doc(key)
                const promise = questionRef.get()
                    .then((question) => {
                        if (!question.exists) {
                            throw ({
                                status: 'fail',
                                message: 'Question Answered of ID ' + key + " does not Exist" 
                            })
                        }
                        if (userAnswers[key] == question.data().answer) score++
                    })
                promises.push(promise)
            })
            await Promise.all(promises)

            const attempt = {
                question_set: questionSet,
                question_group: questionGroup,
                user_email: userEmail,
                user_answers: userAnswers,
                score: score,
                completion_status: completionStatus
            }            
            const result = await db.collection('attempts').add(attempt)

            const accountRef = db.collection('accounts')
            const accounts = await accountRef.where('email', '==', userEmail).get()
            let unlockedMap
            let accountId
            accounts.forEach(async doc => {
                // console.log(doc.data())
                unlockedMap = doc.data()['unlocked_map']
                accountId = doc.data()['uid']
                if (score == 3) {
                    if (questionSet == "DeDnjKAqlzb5cKOJrIzk") {
                        if (!unlockedMap.includes('Map2')) {
                            unlockedMap.push('Map2')
                        }
                    }
                    else if (questionSet == "9vF4uGW4b7oop1EUK1Sm") {
                        if (!unlockedMap.includes('Map3')) {
                            unlockedMap.push('Map3')
                        }
                    }
                    else if (questionSet == "IFBMLqL6Mb16fagAChaO") {
                        if (!unlockedMap.includes('Map4')) {
                            unlockedMap.push('Map4')
                        }
                    }
                    else if (questionSet == "bcO5PTXYNsHaREccZovg") {
                        if (!unlockedMap.includes('Map5')) {
                            unlockedMap.push('Map5')
                        }
                    }
                }
            })
            const acc = db.collection('accounts').doc(accountId)
            const patchBody = {
                'unlocked_map': unlockedMap
            }
            await acc.update(patchBody)

            res.status(200)
            res.json({
                status: 'success',
                data: result.id
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    router.patch('/:id', async (req, res) => {
        const attemptId = req.params.id
        try {
            const attempt = db.collection('attempts').doc(attemptId)
            const doc = await attempt.get()
            if (!doc.exists) {
                throw ({
                    status: 'fail',
                    message: 'Updating Attempt Does Not Exist'
                })
            }
            
            var completionStatus = doc.data().completion_status
            if (completionStatus == COMPLETION_STATUS[1]) {
                throw ({
                    status: 'fail',
                    message: 'Question Set has already been Completed. No Further Updates are Allowed.'
                })
            }

            const userAnswers = req.body.user_answers
            if (userAnswers == null) {
                throw ({
                    status: 'fail',
                    message: 'Only User Answers can be Updated'
                })
            }

            // Get Question Set
            const questionGroup = doc.data().question_group
            const questionSet = doc.data().question_set
            const ref = db.collection(questionGroup.toLowerCase() + 's').doc(questionSet)
            const questionSetInfo = await ref.get()

            // Check Completion Status
            const userAnswerKeys = Object.keys(userAnswers)
            const questionCount = questionSetInfo.data().question_count
            if (userAnswerKeys.length > questionCount) {
                throw ({
                    status: 'fail',
                    message: 'Number of Answers Submitted Exceeds Number of Questions'
                })
            } else if (userAnswerKeys.length < questionCount) {
                completionStatus = COMPLETION_STATUS[0]
            } else {
                completionStatus = COMPLETION_STATUS[1]
            }

            // Calculate Score
            var score = 0
            let promises = []
            userAnswerKeys.forEach(key => {
                const questionRef = db.collection('questions').doc(key)
                const promise = questionRef.get()
                    .then((question) => {
                        if (!question.exists) {
                            throw ({
                                status: 'fail',
                                message: 'Question Answered of ID ' + key + " does not Exist" 
                            })
                        }
                        if (userAnswers[key] == question.data().answer) score++
                    })
                promises.push(promise)
            })
            await Promise.all(promises)

            const patchBody = {
                user_answers: userAnswers,
                completion_status: completionStatus,
                score: score
            }

            const result = await attempt.update(patchBody)
        
            res.status(200)
            res.json({
                status: 'success',
                data: attemptId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    router.delete('/:id', async (req, res) => {
        const attemptId = req.params.id
        try {
            const result = await db.collection('attempts').doc(attemptId).delete()
            
            res.status(200)
            res.json({
                status: 'success',
                data: attemptId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    return router
}