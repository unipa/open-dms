import { lookup } from "./input/lookup.js";
import { select } from "./input/select.js";
import { date } from "./input/date.js";
import { text } from "./input/text.js";

const observer = new MutationObserver((mutations) => {
    // 2: iterate over `MutationRecord` array
    mutations.forEach(mutation => {
        // 3.1: check if the mutation type and the attribute name match
        // 3.2: check if value changed
        mutation.addedNodes.forEach((e,i) =>{
            //debugger;
        if (e.nodeName == "INPUT")
            try{  
            //debugger;      
            var type = e.getAttribute("type");
            if (type == "select") select(e);
            if (type == "lookup") lookup(e);
            if (type == "date") date(e);
            if (type == "textbox") text(e);
        } catch {
            console.log("non entro");
        };
        if (mutation.target.nodeName== "INPUT")
        try {
            var type = e.getAttribute("type");
            if (type == "select") select(e);
            if (type == "lookup") lookup(e);
            if (type == "date") date(e);
            if (type == "textbox") text(e);
        } catch {
            console.log("non entro");
         };
});
        }
        )
        // if (
        //     mutation.type === 'attributes'
        //     && mutation.attributeName === 'value'
        // ) {
        //     // 3.4: trigger `change` event
        //     mutation.target.dispatchEvent(new Event('change'));
        // }
    });


observer.observe(document, { subtree: true, attributes: true, childList: true })

document.querySelectorAll("input[type=lookup]").forEach(element => {
    lookup(element);
});
document.querySelectorAll("input[type=select]").forEach(element => {
    select(element);
});
document.querySelectorAll("input[type=date]").forEach(element => {
    date(element);
});
document.querySelectorAll("input[type=textbox]").forEach(element => {
    text(element);
});

// document.querySelectorAll("input[type=12]").forEach(element => {
//     lookup(element);
// });
// document.querySelectorAll("input[type=10]").forEach(element => {
//     lookup(element);
// });
// document.querySelectorAll("input[type=11]").forEach(element => {
//     lookup(element);
// });
// document.querySelectorAll("input[type=9]").forEach(element => {
//     lookup(element);
// });
