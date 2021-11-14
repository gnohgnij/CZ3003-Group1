const express = require('express')
const router = express.Router()

module.exports = ({ db }) => {
    router.get('/', async (req, res) => {
        try {
            const challenges = db.collection('challenges')
            const challengesSnapshot = await challenges.get()
            const data = []
            challengesSnapshot.forEach(doc => {
                data.push({
                    challenge_id: doc.id,
                    ...doc.data(),
                    deadline: doc.data().deadline.toDate()
                })
            })

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
        const challengeId = req.params.id
        try {
            const challenge = db.collection('challenges').doc(challengeId)
            const doc = await challenge.get()
            var data
            if (doc.exists) {
                data = {
                    challenge_id: doc.id,
                    ...doc.data(),
                    deadline: doc.data().deadline.toDate()
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

    router.get('/email/:email', async (req, res) => {
        try {
            const userEmail = req.params.email
            if (userEmail == null) {
                throw ({
                    status: 'fail',
                    message: 'No user_email Given for Query'
                })
            }
            const challengeRef = db.collection('challenges')
            const challenges = await challengeRef.where('to_email', '==', userEmail).get()
            const data = []
            if (!challenges.empty) {
                challenges.forEach(doc => {
                    data.push({
                        challenge_id: doc.id,
                        ...doc.data(),
                        deadline: doc.data().deadline.toDate()
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

    router.post('/', async (req, res) => {
        try {
            const questionList = req.body.question_list
            if (questionList == null || questionList.length < 1) {
                throw ({
                    status: 'fail',
                    message: 'No Questions Set for the Challenge'
                })
            }
            if (questionList.length < 3) {
                throw ({
                    status: 'fail',
                    message: '3 Questions is Required for the Challenge'
                })
            }

            const questionCount = 3;
            const deadline = new Date()
            deadline.setDate(deadline.getDate() + 7); 

            const fromEmail = req.body.from_email
            if (fromEmail == null) {
                throw ({
                    status: 'fail',
                    message: 'Email of the Person Issuing the Challenge is Required'
                })   
            }
            const toEmail = req.body.to_email

            const challenge = {
                question_count: questionCount,
                deadline: deadline,
                question_list: questionList,
                from_email: fromEmail,
                to_email: toEmail
            }
            const result = await db.collection('challenges').add(challenge)

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

    router.delete('/:id', async (req, res) => {
        const challengeId = req.params.id
        try {
            const result = await db.collection('challenges').doc(challengeId).delete()
            
            res.status(200)
            res.json({
                status: 'success',
                data: challengeId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    return router
}
