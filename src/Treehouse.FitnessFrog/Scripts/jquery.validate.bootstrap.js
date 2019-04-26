(function ($) {
    var defaultOptions = {
        //defining an object with four properties
        //First two, used to update the jQuery validation library as valid an Erro CSS to use the available bootstrap validation classes
        //Next two, define functions that are called by the jQuery validation lirbray when highlighting and un highlighting fields, we need to customize these functions in order to ensure that the valid in error CSS classes are placed on the correct form field element
        //Highlight function uses jquery to find the closest element with a CSS class with a foreign group, then removes the valid class and adds the air class
        //UnHighlight function does the opposite, it finds the closest element with the css class of form-group then removes the errorClass and adds the validClass
        validClass: 'has-success',
        errorClass: 'has-error',
        highlight: function (element, errorClass, validClass) {
            $(element).closest('.form-group')
                .removeClass(validClass)
                .addClass(errorClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).closest('.form-group')
                .removeClass(errorClass)
                .addClass(validClass);
        }
    };

    //Calls jQuery validation libraries setDeaulsts function, passing in our defaultOptions object. Calling this function tells the library to use our default options
    $.validator.setDefaults(defaultOptions);

    //These are set in microsoft jquery unobtrusive validation library options, using default options, object errorClass and validClass properties
    $.validator.unobtrusive.options = {
        errorClass: defaultOptions.errorClass,
        validClass: defaultOptions.validClass,
    };
})(jQuery);