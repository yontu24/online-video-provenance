function movieInfoController() {
    var self = this;
    self.message = 'An Entity of Type: Class.';

    self.$onInit = () => {
        self.movies = [];
        self.isMovieFound = false;
        self.titlesArray = [];
    }
}