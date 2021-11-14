const PORT = process.env.PORT || 3000;
const SERVICE_ACCOUNT = './edumon-edf0b-bf8a3756a40a.json'

// Initialise Express App
const express = require('express')
const app = express()

// Initialise Firebase Firestore
const admin = require('firebase-admin')
const serviceAccount = require(SERVICE_ACCOUNT)
admin.initializeApp({
    credential: admin.credential.cert(serviceAccount)
})
const db = admin.firestore()
db.settings({ ignoreUndefinedProperties: true })

// API Routes
const accountRoutes = require('./routes/accountRoutes')
const gymRoutes = require('./routes/gymRoutes')
const assignmentRoutes = require('./routes/assignmentRoutes')
const attemptRoutes = require('./routes/attemptRoutes')
const challengeRoutes = require('./routes/challengeRoutes')
const questionRoutes = require('./routes/questionRoutes')

app.use(express.urlencoded({ extended: true }))
app.use(express.json())

app.use('/account', accountRoutes({ db }))
app.use('/gym', gymRoutes({ db }))
app.use('/assignment', assignmentRoutes({ db }))
app.use('/attempt', attemptRoutes({ db }))
app.use('/challenge', challengeRoutes({ db }))
app.use('/question', questionRoutes({ db }))

app.listen(PORT, () => {
    console.log('Edumon Server Started at Port ' + PORT)
})