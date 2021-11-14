const { Timestamp } = require('firebase-admin').firestore
const express = require('express')
const router = express.Router()

module.exports = ({ db }) => {
    router.get('/', async (req, res) => {
        try {
            const accounts = db.collection('accounts')
            const accountsSnapshot = await accounts.get()
            const data = []
            accountsSnapshot.forEach( doc => {
                data.push({
                    uid: doc.uid,
                    ...doc.data(),
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
        const accountId = req.params.id
        try {
            const account = db.collection('accounts').doc(accountId)
            const doc = await account.get()
            var data 
            if (doc.exists) {
                data = {
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

    router.get('/email/:email', async (req, res) => {
        const email = req.params.email
        try {
            const accounts = db.collection('accounts')
            const accountsSnapshot = await accounts.where('email', '==', email).get()
            const datas = []
            accountsSnapshot.forEach( doc => {
                datas.push({
                    uid: doc.uid,
                    ...doc.data(),
                })
            })

            const data = datas[0]
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
            const accountId = req.body.id
            const studentId = req.body.studentId
            var data
            if (studentId.length == 0) {
                data = {
                    uid: accountId,
                    username: req.body.username,
                    type: req.body.type,
                    email: req.body.email
                }
            }
            else {
                data = {
                    uid: accountId,
                    username: req.body.username,
                    type: req.body.type,
                    email: req.body.email,
                    studentid: studentId,
                    unlocked_map: ["Map1"]
                }
            }
            const result = await db.collection('accounts').doc(accountId).set({
                ...data
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

    router.post('/username', async (req, res) => {
        try {
            const accountId = req.body.id
            const studentId = req.body.studentId
            var patchBody
            if (studentId.length == 0) {
                patchBody = {
                    username: req.body.username,
                    email: req.body.email
                }
            }
            else {
                patchBody = {
                    username: req.body.username,
                    studentid: studentId,
                    email: req.body.email
                }
            }
            const account = db.collection('accounts').doc(accountId)
            const result = await account.update(patchBody)

            res.status(200)
            res.json({
                status: 'success',
                data: accountId
            })
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })

    router.post('/map', async (req, res) => {
        try {
            const map = req.body.map
            const accountId = req.body.id
            const account = db.collection('accounts').doc(accountId)
            const doc = await account.get()

            var unlockedmap
            if (doc.exists) {
                unlockedmap = doc.data()['unlocked_map']
                unlockedmap.push(map)

                var patchBody = {
                    unlocked_map: unlockedmap
                }
                const result = await account.update(patchBody)
                
                res.status(200)
                res.json({
                    status: 'success',
                    data: accountId
                })
            } else {
                unlockedmap = null

                res.status(200)
                res.json({
                    status: 'fail',
                    message: 'Empty World'
                })
            }
        } catch (err) {
            if (err.status != 'fail') return res.json({ status: 'error', message: err.message })
            res.json(err)
        }
    })


    return router
} 