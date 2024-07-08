"use strict";
var numberOfVoteKaka = 0;
var numberOfVoteCR7 = 0;
var numberOfVoteMessi = 0;

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveVote", function (numberOfVoteKaka, numberOfVoteCR7, numberOfVoteMessi) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `Kaka has ${numberOfVoteKaka} votes.  C.Ronaldo has ${numberOfVoteCR7} votes. Messi has ${numberOfVoteMessi} votes`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {

    //var user = document.getElementById("userInput").value;
    var votedPlayerName = $('input:radio[name=votedPlayer]:checked').val();

        if (votedPlayerName == "Kaka") {
            numberOfVoteKaka++;
        }
        if (votedPlayerName == "C_Ronaldo") {
            numberOfVoteCR7++;
        }
        if (votedPlayerName == "Messi") {
            numberOfVoteMessi++;
        }
        connection.invoke("SendVoteResult", numberOfVoteKaka, numberOfVoteCR7, numberOfVoteMessi).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    
});