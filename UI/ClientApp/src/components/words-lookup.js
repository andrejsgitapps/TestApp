"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WordsLookup = void 0;
var React = require("react");
var react_1 = require("react");
var WordsLookup = function () {
    var apiUrl = 'http://localhost:51187/api/v1/wordslookup/';
    var searchTextMinLength = 3;
    var weightedResultsTop = 5;
    var startMatchAlphaResultsTop = 5;
    var containingMatchAlphaResultsTop = 20;
    var _a = (0, react_1.useState)(''), searchString = _a[0], setSearchString = _a[1];
    var _b = (0, react_1.useState)([]), weightedSuggestions = _b[0], setWeightedSuggestions = _b[1];
    var _c = (0, react_1.useState)([]), startMatchAlphaSuggestions = _c[0], setStartMatchAlphaSuggestions = _c[1];
    var _d = (0, react_1.useState)([]), containMatchAlphaSuggestions = _d[0], setContainMatchAlphaSuggestions = _d[1];
    var lookupWeightedSuggestions = function (searchValue) {
        var lookupModel = { searchString: searchValue, returnTopRecordsCount: weightedResultsTop };
        var json = JSON.stringify(lookupModel);
        fetch(apiUrl + 'lookupweighted', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: json
        })
            .then(function (response) { return response.json(); })
            .then(function (data) {
            var newSuggestions = [];
            for (var i = 0; i < data.length; i++) {
                newSuggestions.push({ id: data[i].id, name: data[i].word });
            }
            setWeightedSuggestions(newSuggestions);
        })
            .catch(function (err) { return console.error(err); });
    };
    var lookupStartMatchAlphaSuggestions = function (searchValue) {
        var lookupModel = { searchString: searchValue, returnTopRecordsCount: startMatchAlphaResultsTop };
        var json = JSON.stringify(lookupModel);
        fetch(apiUrl + 'lookupstartmatchalpha', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: json
        })
            .then(function (response) { return response.json(); })
            .then(function (data) {
            var newSuggestions = [];
            for (var i = 0; i < data.length; i++) {
                newSuggestions.push({ id: data[i].id, name: data[i].word });
            }
            setStartMatchAlphaSuggestions(newSuggestions);
        })
            .catch(function (err) { return console.error(err); });
    };
    var lookupContainMatchAlphaSuggestions = function (searchValue) {
        var lookupModel = { searchString: searchValue, returnTopRecordsCount: containingMatchAlphaResultsTop };
        var json = JSON.stringify(lookupModel);
        fetch(apiUrl + 'lookupcontainingmatchalpha', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: json
        })
            .then(function (response) { return response.json(); })
            .then(function (data) {
            var newSuggestions = [];
            for (var i = 0; i < data.length; i++) {
                newSuggestions.push({ id: data[i].id, name: data[i].word });
            }
            setContainMatchAlphaSuggestions(newSuggestions);
        })
            .catch(function (err) { return console.error(err); });
    };
    var selectSuggestion = function (suggestion, searchString) {
        var selectWordModel = { searchString: searchString, lookupWordId: suggestion.id };
        var json = JSON.stringify(selectWordModel);
        return fetch(apiUrl + 'selectword', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: json
        }).then(function (response) { return response.json(); })
            .then(function (data) {
            reloadAllSuggestions(searchString);
        })
            .catch(function (err) { return console.error(err); });
    };
    var reloadAllSuggestions = function (value) {
        lookupWeightedSuggestions(value);
        lookupStartMatchAlphaSuggestions(value);
        lookupContainMatchAlphaSuggestions(value);
    };
    var onTextChange = function (e) {
        var value = e.target.value;
        if (value.length >= searchTextMinLength) {
            reloadAllSuggestions(value);
        }
        else {
            setWeightedSuggestions([]);
            setStartMatchAlphaSuggestions([]);
            setContainMatchAlphaSuggestions([]);
        }
        setSearchString(value);
    };
    var renderSuggestions = function () {
        if (searchString.length >= searchTextMinLength) {
            var weightedSuggestionsList = void 0;
            if (weightedSuggestions.length === 0) {
                weightedSuggestionsList = React.createElement("li", null, "Nothing found in weighted lookup.");
            }
            else {
                weightedSuggestionsList = weightedSuggestions.map(function (suggestion) { return React.createElement("li", { key: suggestion.id, onClick: function (e) { return selectSuggestion(suggestion, searchString); } }, suggestion.name); });
            }
            var startMatchAlphaSuggestionsList = void 0;
            if (startMatchAlphaSuggestions.length === 0) {
                startMatchAlphaSuggestionsList = React.createElement("li", null, "Nothing found in start match alphabetical lookup.");
            }
            else {
                startMatchAlphaSuggestionsList = startMatchAlphaSuggestions.map(function (suggestion) { return React.createElement("li", { key: suggestion.id, onClick: function (e) { return selectSuggestion(suggestion, searchString); } }, suggestion.name); });
            }
            var containMatchAlphaSuggestionsList = void 0;
            if (containMatchAlphaSuggestions.length === 0) {
                containMatchAlphaSuggestionsList = React.createElement("li", null, "Nothing found in containing match alphabetical lookup.");
            }
            else {
                containMatchAlphaSuggestionsList = containMatchAlphaSuggestions.map(function (suggestion) { return React.createElement("li", { key: suggestion.id, onClick: function (e) { return selectSuggestion(suggestion, searchString); } }, suggestion.name); });
            }
            return (React.createElement("ul", null,
                React.createElement("lh", null,
                    React.createElement("b", null, "Weighted lookup:")),
                weightedSuggestionsList,
                React.createElement("lh", null,
                    React.createElement("b", null, "Start Match Alphabetical lookup:")),
                startMatchAlphaSuggestionsList,
                React.createElement("lh", null,
                    React.createElement("b", null, "Containing Match Alphabetical lookup:")),
                containMatchAlphaSuggestionsList));
        }
    };
    return React.createElement("div", null,
        "Enter word to search: ",
        React.createElement("br", null),
        React.createElement("input", { onChange: onTextChange, value: searchString, type: "text", placeholder: "Start typing word here..." }),
        renderSuggestions());
};
exports.WordsLookup = WordsLookup;
//# sourceMappingURL=words-lookup.js.map