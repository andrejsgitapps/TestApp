import * as React from 'react';
import { useState } from "react";

interface Suggestion {
    id: Number;
    name: String;
}

interface LookupModel {
    searchString: String;
    returnTopRecordsCount: Number;
}

interface SelectWordModel {
    searchString: String;
    lookupWordId: Number;
}

export const WordsLookup = () => {
    const apiUrl = 'http://localhost:51187/api/v1/wordslookup/';
    const searchTextMinLength = 3;
    const weightedResultsTop = 5;
    const startMatchAlphaResultsTop = 5;
    const containingMatchAlphaResultsTop = 20;

    const [searchString, setSearchString] = useState<String>('');    
    const [weightedSuggestions, setWeightedSuggestions] = useState<Suggestion[]>([]);
    const [startMatchAlphaSuggestions, setStartMatchAlphaSuggestions] = useState<Suggestion[]>([]);
    const [containMatchAlphaSuggestions, setContainMatchAlphaSuggestions] = useState<Suggestion[]>([]);    

    const lookupWeightedSuggestions = (searchValue) => {        
        const lookupModel: LookupModel = { searchString: searchValue, returnTopRecordsCount: weightedResultsTop }
        const json = JSON.stringify(lookupModel);                        

        fetch(apiUrl + 'lookupweighted', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json'},
                body: json
            })
            .then(response => response.json())
            .then(data => {
                let newSuggestions: Suggestion[] = [];                
                for (var i = 0; i < data.length; i++) {
                    newSuggestions.push({ id: data[i].id, name: data[i].word });
                }
                setWeightedSuggestions(newSuggestions);                
            })
            .catch(err => console.error(err));
    }

    const lookupStartMatchAlphaSuggestions = (searchValue) => {
        const lookupModel: LookupModel = { searchString: searchValue, returnTopRecordsCount: startMatchAlphaResultsTop }
        const json = JSON.stringify(lookupModel);

        fetch(apiUrl + 'lookupstartmatchalpha', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: json
        })
            .then(response => response.json())
            .then(data => {
                let newSuggestions: Suggestion[] = [];
                for (var i = 0; i < data.length; i++) {
                    newSuggestions.push({ id: data[i].id, name: data[i].word });
                }
                setStartMatchAlphaSuggestions(newSuggestions);                
            })
            .catch(err => console.error(err));
    }

    const lookupContainMatchAlphaSuggestions = (searchValue) => {
        const lookupModel: LookupModel = { searchString: searchValue, returnTopRecordsCount: containingMatchAlphaResultsTop }
        const json = JSON.stringify(lookupModel);

        fetch(apiUrl + 'lookupcontainingmatchalpha', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: json
        })
            .then(response => response.json())
            .then(data => {
                let newSuggestions: Suggestion[] = [];
                for (var i = 0; i < data.length; i++) {
                    newSuggestions.push({ id: data[i].id, name: data[i].word });
                }
                setContainMatchAlphaSuggestions(newSuggestions);                
            })
            .catch(err => console.error(err));
    }

    const selectSuggestion = (suggestion: Suggestion, searchString: String) => {
        const selectWordModel: SelectWordModel = { searchString: searchString, lookupWordId: suggestion.id }
        const json = JSON.stringify(selectWordModel);        

        return fetch(apiUrl + 'selectword', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: json
        }).then(response => response.json())
            .then(data => {
                reloadAllSuggestions(searchString);
            })
            .catch(err => console.error(err));
    }

    const reloadAllSuggestions = (value: String) => {
        lookupWeightedSuggestions(value);
        lookupStartMatchAlphaSuggestions(value);
        lookupContainMatchAlphaSuggestions(value);
    }

    const onTextChange = (e) => {
        const value = e.target.value;
        if (value.length >= searchTextMinLength) {
            reloadAllSuggestions(value);
        } else {
            setWeightedSuggestions([]);
            setStartMatchAlphaSuggestions([]);
            setContainMatchAlphaSuggestions([]);
        }
        setSearchString(value);
    }

    const renderSuggestions = () => {
        if (searchString.length >= searchTextMinLength) {

            let weightedSuggestionsList: any;
            if (weightedSuggestions.length === 0) {
                weightedSuggestionsList = <li>Nothing found in weighted lookup.</li>
            } else {
                weightedSuggestionsList = weightedSuggestions.map(suggestion => <li key={suggestion.id} onClick={(e) => selectSuggestion(suggestion, searchString)}>{suggestion.name}</li>)
            }

            let startMatchAlphaSuggestionsList: any;
            if (startMatchAlphaSuggestions.length === 0) {
                startMatchAlphaSuggestionsList = <li>Nothing found in start match alphabetical lookup.</li>
            } else {
                startMatchAlphaSuggestionsList = startMatchAlphaSuggestions.map(suggestion => <li key={suggestion.id} onClick={(e) => selectSuggestion(suggestion, searchString)}>{suggestion.name}</li>)
            }

            let containMatchAlphaSuggestionsList: any;
            if (containMatchAlphaSuggestions.length === 0) {
                containMatchAlphaSuggestionsList = <li>Nothing found in containing match alphabetical lookup.</li>
            } else {
                containMatchAlphaSuggestionsList = containMatchAlphaSuggestions.map(suggestion => <li key={suggestion.id} onClick={(e) => selectSuggestion(suggestion, searchString)}>{suggestion.name}</li>)
            }

            return (
                <ul>                    
                    <lh><b>Weighted lookup:</b></lh>
                    {weightedSuggestionsList}
                    <lh><b>Start Match Alphabetical lookup:</b></lh>
                    {startMatchAlphaSuggestionsList}
                    <lh><b>Containing Match Alphabetical lookup:</b></lh>
                    {containMatchAlphaSuggestionsList}
                </ul>
            )
        }
    }

    return <div>
        Enter word to search: <br />
        <input onChange={onTextChange} value={searchString} type="text" placeholder="Start typing word here..." />
        {renderSuggestions()}
        </div>
}