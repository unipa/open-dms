declare class EditFormFieldHandler {
    /**
     * @constructor
     * @param { import('../../../FormEditor').default } formEditor
     * @param { import('../../../core/FormFieldRegistry').default } formFieldRegistry
     */
    constructor(formEditor: import('../../../FormEditor').default, formFieldRegistry: import('../../../core/FormFieldRegistry').default);
    _formEditor: import("../../../FormEditor").default;
    _formFieldRegistry: import("../../../core/FormFieldRegistry").default;
    execute(context: any): any;
    revert(context: any): any;
}
declare namespace EditFormFieldHandler {
    const $inject: string[];
}
export default EditFormFieldHandler;
