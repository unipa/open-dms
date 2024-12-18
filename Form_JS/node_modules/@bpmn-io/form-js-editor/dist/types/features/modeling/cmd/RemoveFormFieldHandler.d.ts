declare class RemoveFormFieldHandler {
    /**
     * @constructor
     * @param { import('../../../FormEditor').default } formEditor
     * @param { import('../../../core/FormFieldRegistry').default } formFieldRegistry
     */
    constructor(formEditor: import('../../../FormEditor').default, formFieldRegistry: import('../../../core/FormFieldRegistry').default);
    _formEditor: import("../../../FormEditor").default;
    _formFieldRegistry: import("../../../core/FormFieldRegistry").default;
    execute(context: any): void;
    revert(context: any): void;
}
declare namespace RemoveFormFieldHandler {
    const $inject: string[];
}
export default RemoveFormFieldHandler;
