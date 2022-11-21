function treeSelectNode(treeview, node, triggerSelect) {
    if (triggerSelect == null) triggerSelect = true;
    treeview.select(node);
    if (triggerSelect) treeview.trigger("select", { node: node });
}

function treeNode(treeview, path, callback) {

    var ds = treeview.dataSource;
    var node = ds.get(path[0])

    // skip already expanded and loaded nodes
    while (path.length > 1 && (node.expanded || node.loaded())) {
        //while (path.length > 1 && node.expanded && node.loaded()) {
        node.set("expanded", true);
        path.shift();
        node = ds.get(path[0]);
    }

    // if there are levels to expand, expand them
    if (path.length > 1) {

        node = null;

        // listen to the change event to know when the node has been loaded
        ds.bind("change", function expandLevel(e) {
            var id = e.node && e.node.id;

            // proceed if the change is caused by the last fetching
            if (id == path[0]) {
                path.shift();
                ds.unbind("change", expandLevel);

                treeNode(treeview, path, callback);
            }
        });

        ds.get(path[0]).set("expanded", true);
    } else {
        // otherwise select
        node = treeview.findByUid(ds.get(path[0]).uid);
        if (callback != null && callback != "") eval(callback + "(node)");
        //treeview.element.closest(".k-scrollable").scrollTo(treeview.select(), 400);
    }

    return node;
}

function treePathToTreePath(sPath) {
    var arrPath = sPath.split("/");
    var arrTreePath = [];
    for (var i = 0; i < arrPath.length; i++) {
        arrTreePath = arrTreePath.concat(arrPath[i]);
    }
    return arrTreePath;
}
