const express = require('express')
const router = express.Router()

module.exports = ({ db }) => {
    router.get('/', async (req, res) => {
        try {
            const gyms = db.collection('gyms')
            const gymsSnapshot = await gyms.get()
            const data = []
            gymsSnapshot.forEach(doc => {
                data.push({
                    gym_id: doc.id,
                    ...doc.data()
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
        const gymId = req.params.id
        try {
            const gym = db.collection('gyms').doc(gymId)
            const doc = await gym.get()
            var data
            if (doc.exists) {
                data = {
                    gym_id: doc.id,
                    ...doc.data()
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
            const questionList = req.body.question_list
            if (questionList == null || questionList.length < 1) {
                throw ({
                    status: 'fail',
                    message: 'No Questions Set for this Gym'
                })
            }
            const passingScore = req.body.passing_score
            if (passingScore == null) {
                throw ({
                    status: 'fail',
                    message: 'No Psssing Score Set for this Gym'
                })
            }

            const gym = {
                question_count: questionList.length,
                question_list: questionList,
                passing_score: passingScore
            }
            const result = await db.collection('gyms').add(gym)

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
        const gymId = req.params.id
        const patchBody = {} 
        const questionList = req.body.question_list
        if (questionList != null) {
            patchBody.question_list = questionList
            patchBody.question_count = questionList.length
        }
        const passingScore = req.body.passing_score
        if (passingScore != null) {
            patchBody.passing_score = passingScore
        }

        try {
            const gym = db.collection('gyms').doc(gymId)
            const result = await gym.update(patchBody)

            res.status(200)
            res.json({
                status: 'success',
                data: gymId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    router.delete('/:id', async (req, res) => {
        const gymId = req.params.id
        try {
            const result = await db.collection('gyms').doc(gymId).delete()
            
            res.status(200)
            res.json({
                status: 'success',
                data: gymId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    return router
}