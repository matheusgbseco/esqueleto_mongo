import firebase from 'firebase/app';
import "firebase/auth";
import "firebase/storage";
import "firebase/analytics";

let config = {
    apiKey: "",
    authDomain: "",
    projectId: "",
    storageBucket: "",
    messagingSenderId: "",
    appId: "",
    measurementId: ""
};

firebase.initializeApp(config);
firebase.analytics();

export default firebase;