jQuery.extend(jQuery.validator.messages, {
    required: "Ce champ {0} est requis.",
    Remote: "Veuillez corriger ce champ.",
    email: "S'il vous plaît, mettez une adresse email valide.",
    url: "Veuillez saisir une URL valide.",
    date: "Veuillez saisir une date valide.",
    dateISO: "Veuillez saisir une date valide (ISO).",
    number: "Veuillez entrer un numéro valide.",
    digits: "Veuillez entrer uniquement des chiffres.",
    creditcard: "Veuillez entrer un numéro de carte de crédit valide.",
    equalTo: "Veuillez saisir à nouveau la même valeur.",
    accept: "Veuillez saisir une valeur avec une extension valide.",
    maxlength: jQuery.validator.format ("N'entrez pas plus de {0} caractères."),
    minlength: jQuery.validator.format ("Veuillez entrer au moins {0} caractères."),
    rangelength: jQuery.validator.format ("Entrez une valeur entre {0} et {1} caractères long."),
    range: jQuery.validator.format ("Entrez une valeur entre {0} et {1}."),
    max: jQuery.validator.format ("Veuillez entrer une valeur inférieure ou égale à {0}."),
    min: jQuery.validator.format ("Veuillez entrer une valeur supérieure ou égale à {0}.")
});