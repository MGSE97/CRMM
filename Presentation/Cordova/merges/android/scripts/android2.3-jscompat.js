// Kód pro podporu Function.prototype.bind() na Androidu 2.3
(function () {
    if (!Function.prototype.bind) {
        Function.prototype.bind = function (thisValue) {
            if (typeof this !== "function") {
                throw new TypeError(this + " cannot be bound as it is not a function");
            }

            // bind() taky umožňuje předřadit k volání argumenty.
            var preArgs = Array.prototype.slice.call(arguments, 1);

            // Skutečná funkce k vytvoření vazby hodnoty this a argumentů
            var functionToBind = this;
            var noOpFunction = function () { };

            // Argument this, který se má použít
            var thisArg = this instanceof noOpFunction && thisValue ? this : thisValue;

            // Výsledná vázaná funkce
            var boundFunction = function () {
                return functionToBind.apply(thisArg, preArgs.concat(Array.prototype.slice.call(arguments)));
            };

            noOpFunction.prototype = this.prototype;
            boundFunction.prototype = new noOpFunction();

            return boundFunction;
        };
    }
}());
