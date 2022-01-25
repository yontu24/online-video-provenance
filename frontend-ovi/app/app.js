var app = angular.module('ovi', [
    'ngAnimate'
]).filter('range', () => {
    return (input, total) => {
        total = parseInt(total);
        for (var i = 0; i < total; i++)
            input.push(i);

        return input;
    }
});
