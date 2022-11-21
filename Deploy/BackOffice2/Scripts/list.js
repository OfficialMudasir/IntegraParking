function list() {

    this.list = new Array();

    this.add = function (item) {

        this.list.push(item);
    }

    this.getItemById = function (id) {

        for (var i = 0; i < this.list.length; i++) {

            var item = this.list[i];

            if (item.equalId(id)) {

                return item;
            }
        }

        return null;

    }

    this.getItemByName = function (name) {

        for (var i = 0; i < this.list.length; i++) {

            var item = this.list[i];

            if (item.equalName(name)) {

                return item;
            }
        }

        return null;
    }

    this.deleteById = function (id) {

        var tmp_list = new Array();

        for (var i = 0; i < this.list.length; i++) {

            var item = this.list[i];

            if (!item.equalId(id)) {

                tmp_list.push(item);
            }

        }
        this.list = tmp_list;
    }

    this.addInstance = function (target) {
        if (!this.existInstance(target)) {
            this.list.push(target);
        }
    }

    this.existInstance = function (target) {
        for (var i = 0; i < this.list.length; i++) {
            var item = this.list[i];
            if (item == target) {
                return true;
            }
        }
        return false;
    }

    this.clear = function () {
        if (!this.isEmpty())
            this.list = [];
    }

    this.isEmpty = function () {
        return (this.list.length == 0);
    }

}