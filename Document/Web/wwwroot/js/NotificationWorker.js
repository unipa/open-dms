
var badgeValues = new Object();

function Check(bag, firstTime, time) {
    fetch(bag.url)
        .then(response => response.json())
        .then(data => {
            if (firstTime || badgeValues[bag.badge] != data) {
                var diff = firstTime ? data : data - badgeValues[bag.badge];
                var msg = bag.message.replace("{0}", diff);
                postMessage({ op: firstTime ? "badge-read" : "badge-changed", badge: bag.badge, value: data, changed: diff, message: msg, pageURL: bag.pageURL });
                badgeValues[bag.badge] = data;
            }
            setTimeout(() => { Check(bag, false, time); }, time);
        })
        .catch(err => {
            postMessage({ op: "badge-error", badge: bag.badge, value: 0, changed: 0, message: "", pageURL: bag.pageURL });
            setTimeout(() => { Check(bag, false, time); }, time * 2);
        });
}


onmessage = function (e) {
    console.log('NotificationWorker Started');
    var bag = {
        badge : e.data.badge,
        url : e.data.url,
        message : e.data.message,
        pageURL: e.data.pageURL
    }
    Check(bag, true, 60000);
}