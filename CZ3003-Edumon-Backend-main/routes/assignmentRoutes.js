const express = require('express')
const router = express.Router()

module.exports = ({ db }) => {
    router.get('/', async (req, res) => {
        try {
            const assignments = db.collection('assignments')
            const assignmentsSnapshot = await assignments.get()
            const data = []
            assignmentsSnapshot.forEach(doc => {
                data.push({
                    assignment_id: doc.id,
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
        const assignmentId = req.params.id
        try {
            const assignment = db.collection('assignments').doc(assignmentId)
            const doc = await assignment.get()
            var data
            if (doc.exists) {
                data = {
                    assignment_id: doc.id,
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

    router.post('/', async (req, res) => {
        try {
            const deadline = req.body.deadline
            if (deadline == null) {
                throw ({
                    status: 'fail',
                    message: 'No Deadline Set for the Assignment'
                })
            }
            const questionList = req.body.question_list 
            if (questionList == null || questionList.length < 1) {
                throw ({
                    status: 'fail',
                    message: 'No Questions Set for the Assignment'
                })
            }
            const questionCount = questionList.length 

            const assignment = {
                deadline: new Date(deadline),
                question_count: questionCount,
                question_list: questionList,
            }
            const result = await db.collection('assignments').add(assignment)

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
        const assignmentId = req.params.id
        const patchBody = {} 
        const deadline = req.body.deadline
        if (deadline != null) patchBody.deadline = new Date(deadline)
        const questionList = req.body.question_list
        if (questionList != null) {
            patchBody.question_list = questionList
            patchBody.question_count = questionList.length
        }
        try {
            const assignment = db.collection('assignments').doc(assignmentId)
            const result = await assignment.update(patchBody)

            res.status(200)
            res.json({
                status: 'success',
                data: assignmentId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    router.delete('/:id', async (req, res) => {
        const assignmentId = req.params.id
        try {
            const result = await db.collection('assignments').doc(assignmentId).delete()
            
            res.status(200)
            res.json({
                status: 'success',
                data: assignmentId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    return router
}

