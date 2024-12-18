import { lookup } from "./input/lookup.js";
import { select } from "./input/select.js";
import { date } from "./input/date.js";
import { text } from "./input/text.js";

function isHidden(el) {
    var style = window.getComputedStyle(el);
    return (
        (style.display === 'none')
        || (style.visibility === 'hidden')
        || (el.classList.contains('lookup-input')));
}

function CheckControl(e) {
//    if (e.nodeName == "INPUT" && !isHidden(e)) {
    if (e.nodeName == "INPUT" && !e.name.endsWith("_description")) {
        try {
            let type = e.getAttribute("type");
            if (type == "select") select(e);
            if (type == "lookup") lookup(e);
            if (type == "date") date(e);
            if (type == "textbox") text(e);
        } catch { };
        return true;
    }
    return false;
}

const observer = new MutationObserver((mutations) => {
    // 2: iterate over `MutationRecord` array
    mutations.forEach(mutation => {
        // 3.1: check if the mutation type and the attribute name match
        // 3.2: check if value changed
        mutation.addedNodes.forEach((e, i) => {
            if (!CheckControl(e)) {
                if (e.nodeName == "FORM" && !isHidden(e)) {
                    e.querySelectorAll('INPUT').forEach((ei, i) => {
                        CheckControl(ei)
                    })
                }
            }
        });
       if (mutation.target) {
            if (!CheckControl(mutation.target)) {
                if (mutation.target.nodeName == "FORM" && !isHidden(mutation.target)) {
                    mutation.target.querySelectorAll('INPUT').forEach((ei, i) => {
                        CheckControl(ei)
                    })
                }
            }
        }
    });
});

export function dataEntry() {

    Array.from(document.querySelectorAll("input[type=lookup]")).filter(e => !isHidden(e)).forEach(element => {
        lookup(element);
    });
    Array.from(document.querySelectorAll("input[type=select]")).filter(e => !isHidden(e)).forEach(element => {
        select(element);
    });
    Array.from(document.querySelectorAll("input[type=date]")).filter(e => !isHidden(e)).forEach(element => {
        date(element);
    });
    Array.from(document.querySelectorAll("input[type=textbox]")).filter(e => !isHidden(e)).forEach(element => {
        text(element);
    });

}

dataEntry();
observer.observe(document, { subtree: true, attributes: true, childList: true })

