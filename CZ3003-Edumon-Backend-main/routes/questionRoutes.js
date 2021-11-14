const express = require('express')
const { firestore } = require('firebase-admin')
const router = express.Router()

module.exports = ({ db }) => {
    router.get('/:id', async (req, res) => {
        const questionId = req.params.id
        try {
            const question = db.collection('questions').doc(questionId)
            const doc = await question.get()
            var data
            if (doc.exists) {
                data = {
                    question_id: doc.id,
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
            const questionText = req.body.question_text
            if (questionText == null) {
                throw ({
                    status: 'fail',
                    message: 'No Question Text Submitted'
                })
            }
            const optionOne = req.body.option_one
            const optionTwo = req.body.option_two
            if (optionOne == null || optionTwo == null) {
                throw ({
                    status: 'fail',
                    message: 'Missing option_one or option_two Field'
                })
            }
            if (typeof(optionOne) != 'string' || typeof(optionOne) != 'string') {
                throw ({
                    status: 'fail',
                    message: 'Option Fields should be a String'
                })
            }
            var optionCount = 2
            const optionThree = req.body.option_three
            if (optionThree != null) optionCount++
            const optionFour = req.body.option_four
            if (optionFour != null && optionCount < 3) {
                throw ({
                    status: 'fail',
                    message: 'Missing option_three Field'
                })
            } else optionCount++
            const optionFive = req.body.option_five
            if (optionFive != null && optionCount < 4) {
                throw ({
                    status: 'fail',
                    message: 'Missing option_four Field'
                })
            } else optionCount++
            const answer = parseInt(req.body.answer)
            if (answer < 1 || answer > optionCount) {
                throw ({
                    status: 'fail',
                    message: 'Invalid Answer. Answer should be an integer between 1 and ' + optionCount + ' (inclusive).' 
                })
            }

            const question = {
                question_text: questionText,
                option_one: optionOne,
                option_two: optionTwo,
                option_three: optionThree,
                option_four: optionFour,
                option_five: optionFive,
                answer
            }
            const result = await db.collection('questions').add(question)

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

    // Can be Updated for Error Checks
    router.put('/:id', async (req, res) => {
        const questionId = req.params.id
        const patchBody = {
            ...req.body
        }
        try {
            const question = db.collection('questions').doc(questionId)
            const result = await question.update(patchBody)

            const optionThree = req.body.option_three
            const optionFour = req.body.option_four
            const optionFive = req.body.option_five
            if (optionThree == null) {
                result = await question.update(
                    { "option_three": firestore.FieldValue.delete(), 
                    "option_four": firestore.FieldValue.delete(), 
                    "option_five": firestore.FieldValue.delete() })
            } 
            else if (optionFour == null) {
                result = await question.update(
                    { "option_four": firestore.FieldValue.delete(), 
                    "option_five": firestore.FieldValue.delete() })
            }
            else if (optionFive == null) result = await question.update({ "option_five": firestore.FieldValue.delete() })

            res.status(200)
            res.json({
                status: 'success',
                data: questionId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    router.delete('/:id', async (req, res) => {
        const questionId = req.params.id
        try {
            const result = await db.collection('questions').doc(questionId).delete()
            
            res.status(200)
            res.json({
                status: 'success',
                data: questionId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    return router
}