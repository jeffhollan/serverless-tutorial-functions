var https = require('https');

module.exports = function (context, data) {
    context.log('Webhook was triggered!');

    var email = `<body><h1>Hello ${data.Name}</h1><p>Your book is due on ${data.Date}</p></body>`
    context.res = {
        body: email
    }
    context.done();

}
