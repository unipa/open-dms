export default class MixinStyle extends HTMLElement {
  constructor(root) {
    super();
    this.root = root;
    const style = document.createElement("style");
    style.innerHTML = `
      
  /* --- Label --- */
label {
font-size: 16px;
display: block;
font-weight: 500;
}

/* --- Search Bar --- */
input, textarea, .search-bar, .date-field {
  width: 100%;
  box-sizing: border-box
}
.search-bar input {
border: none;
background: transparent;
margin: 0;
padding: 10px;
font-size: 14px;
color: inherit;
border: 1px solid transparent;
border-radius: inherit;
width: 100%;
min-width: 30%;
}

/* --- Scroll bar --- */
.search-bar::-webkit-scrollbar-track
{
	-webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
	border-radius: 10px;
	background-color: #F5F5F5;
}

.search-bar::-webkit-scrollbar
{
	height: 8px;
	background-color: #F5F5F5;
}

.search-bar::-webkit-scrollbar-thumb
{
	border-radius: 10px;
	-webkit-box-shadow: inset 0 0 6px rgba(0,0,0,.3);
	background-color: #848C8E;
}


.search-box {
display: flex;
background-color: #eee;
border-bottom: 1px solid #5d7083;
}
.search-bar {
  display: flex;
  overflow-x:auto;
  padding-left:5px;
  }
  
.multiselect-array {
  display:flex; 
  align-items:center; 
  
}
.toggle-button {
background-color: none;
border: none;
cursor: pointer;
font-size: 14px;
height: auto;
padding: 10px;
width: 30px;
height: auto;
text-align: center;
transform: rotate(0deg);
transition: transform 0.4s ease-in-out;
}

.toggle-button svg {
width: 20px;
height: 20px;
fill: #000;
}

.chevron-active {
transform: rotate(180deg);
transition: transform 0.4s ease-in-out;

}

.parent-item {
display: flex;
flex-direction: row;
justify-content: space-between;
}


.selectable-item {
  padding-top: 2px;
  margin: 0;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  padding-right: 2px;
}


    .selectable-item span {
        font-weight: 600;
        color: #222;
    }

        .selectable-item small {
            font-size: .85em;
            color: #777;
        }


.selectable-with-chevron {
  width: 90%;
}

.selectable-without-chevron {
 pading-right: 10%;
}

.search-bar input[type="search"]::placeholder {
color: #bbb;
}

.search-results {
position: relative;

}

.search-results ul {
background-color: #eee;
animation: slide-down .6s cubic-bezier(0.4, 0, 0.2, 1) 500ms;
list-style: none;
margin: 2px 0;
position: absolute;
top: 0;
left: 0;
right: 0;
z-index: 100;
border: 1px solid #ccc;
border-top: none;
border-radius: 0 0 5px 5px;
padding: 0 10px;
box-shadow: 0 0 3px 0 #ccc;
animation-fill-mode: both;

}

ul.child-list {
position: static;
box-shadow: none;
border: none;
height: 0;
display: block;
overflow: hidden;
transition: height 0.5s ease-in-out;

}

ul.is-visible {
height: auto;
}

.search-results ul li {
padding: 5px 0;
border-bottom: 1px solid #ccc;
cursor: pointer;
}

.search-results ul li:last-child {
border-bottom: none;
padding-bottom: 20px;
}

.search-results ul li a {
color: #000;
text-decoration: none;
}

.search-results ul li a:hover {
text-decoration: underline;
}

.search-results ul li a:focus {
color: #1183d6;
}

.search-box button {
  cursor: auto;
text-indent: -999px;
overflow: hidden;
width: 35px;
padding: 0;
margin: 0;
border: 1px solid transparent;
border-radius: inherit;
background: transparent url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='16' height='16' class='bi bi-search' viewBox='0 0 16 16'%3E%3Cpath d='M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z'%3E%3C/path%3E%3C/svg%3E") no-repeat center;
opacity: 0.7;
}
.unavailable {
  color:red;
}

.search-bar button:hover {
opacity: 1;
}

.search-bar button:focus,
.search-bar input[type="search"]:focus {
outline: none;
}

.search-bar form.nosubmit {
border: none;
padding: 0;
}

.search-bar input.nosubmit {
width: 260px;
border: 1px solid #555;
padding: 9px 4px 9px 40px;
background: transparent url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='16' height='16' class='bi bi-search' viewBox='0 0 16 16'%3E%3Cpath d='M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z'%3E%3C/path%3E%3C/svg%3E") no-repeat 13px center;
}

.search-bar input {
    border: none;
    padding: 0.375rem 0.5rem;
    outline: 0;
    height: 2.5rem;
    width: 100%;
    box-shadow: none;
    transition: none;
    font-weight: 700;
    -webkit-appearance: none;
    -webkit-border-radius: 0;
}

.multiselect-item {
  padding:0 5px;
  font-size:inherit;
  color:inherit;
  font-family:inherit;
  display:flex;
  flex-direction: row; 
  align-items:center;
  cursor:pointer;
}

.delete-icon {
  width:auto;
  height:15px;
  display:block;
}
/* --- Text Area --- */
.textarea-field textarea {
border: 1px solid #B9BCB3;
border-radius: 5px;
padding: 9px 5px;
margin:auto;
min-height: 50px;
max-height: 200px;
font-size: 14px;
resize: vertical;
}

.textarea-field textarea:focus {
box-shadow: 0 0 3px 0 #1183d6;
border-color: #1183d6;
outline: none;
}

.textarea-field textarea::placeholder {
color: #bbb;
}

/* --- Text Input --- */
.text-field input {
border: 1px solid #B9BCB3;
border-radius: 5px;
padding: 6px 5px;

;
}

.text-field input:focus {
box-shadow: 0 0 3px 0 #1183d6;
border-color: #1183d6;
outline: none;
}

.text-field input::placeholder {
color: #bbb;
}
/* --- Number Input --- */
.number-field input {
border: 1px solid #B9BCB3;
border-radius: 5px;
padding: 11px 5px;

;
}

.number-field input:focus {
box-shadow: 0 0 3px 0 #1183d6;
border-color: #1183d6;
outline: none;
}

.number-field input::placeholder {
color: #bbb;
}


/* DatePicker */
input[type="date"] {
margin: 10px 0;
height: 40px;
padding-left: 10px;
font-size:14px;
position:relative;
font-family:monospace;
border:1px solid #B9BCB3;
border-radius:0.25rem;
background:
  white
  url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='20' height='22' viewBox='0 0 20 22'%3E%3Cg fill='none' fill-rule='evenodd' stroke='grey' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' transform='translate(1 1)'%3E%3Crect width='18' height='18' y='2' rx='2'/%3E%3Cpath d='M13 0L13 4M5 0L5 4M0 8L18 8'/%3E%3C/g%3E%3C/svg%3E")
  right 1rem
  center
  no-repeat;

cursor:pointer;
}
::-webkit-calendar-picker-indicator {
  width: 40px;
}

::-webkit-clear-button,
::-webkit-inner-spin-button {
display:none;
}
::-webkit-calendar-picker-indicator {
  opacity: 0;
  cursor: pointer;
}
/* Animations */


@keyframes slide-down {
0% {
  transform: translateY(-20%);
  opacity: 0;
}

100% {
  transform: translateY(0);
  opacity: 1;
}
}

@keyframes slide-down-opacity {
0% {
  transform: translateY(-20%);
  opacity: 0;
}

100% {
  transform: translateY(0);

}
}`;
    this.appendChild(style);
  }
}
customElements.define("mixin-style", MixinStyle);
