function triCheckbox_Init() {

    $('input[type="checkbox"].tri-check').each(function () {
        // keep a reference to the current checkbox
        var me = $(this);
        // define the three states our checkbox can enter:
        // checked, not checked, and faded out
        var states = [
            function () {
                me.val(true);
                me.attr('checked', 'checked');
                me.css({ opacity: 1 });
            },
            function () {
                me.val(false);
                me.removeAttr('checked');
                me.css({ opacity: 1 });
            },
            function () {
                me.val('null');
                me.removeAttr('checked');
                me.css({ opacity: 0.5 });
                me.prop('indeterminate', true);
            }
        ];
        console.log(states)
        // start off in the not checked state
        var currentState = 1;
        // whenever the checkbox is changed, loop through our checkbox states
        me.change(function () {
            //console.log('me.change' + "**" + currentState);
            //states[++currentState > 2 ? currentState = 0 : currentState]();
        });
        me.click(function () {
            states[++currentState > 2 ? currentState = 0 : currentState]();
        });
        // force it into the next state:
        // I want it to start off in the faded out state (currentState == 2)
        me.change();
        //console.log('last');
    });

}