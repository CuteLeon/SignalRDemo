const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", (user, message) => {
    //alert("收到一条消息耶！！！\n" + user + ":\n" + message)
    const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    const encodedMsg = user + " says " + msg;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().catch(err => console.error(err.toString()));

/*
var ele = document.getElementById("sendButton");
ele.onclick = function () { alert(".onclick") };
ele.click = function () { alert(".click") };
ele.attachEvent("onclick", function () { alert("attachEvent(onclick)") });
ele.attachEvent("click", function () { alert("attachEvent(click)") });
ele["click"] = function () { alert("[click]") };
ele["onclick"] = function () { alert("[onclick]") };
 */

document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    //alert("发送消息：\n" + user + ":\n" + message)
    connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
    event.preventDefault();
});