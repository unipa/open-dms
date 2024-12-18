declare class UpdateIdClaimHandler {
    /**
     * @constructor
     * @param { import('../../../core/FormFieldRegistry').default } formFieldRegistry
     */
    constructor(formFieldRegistry: import('../../../core/FormFieldRegistry').default);
    _formFieldRegistry: import("../../../core/FormFieldRegistry").default;
    execute(context: any): void;
    revert(context: any): void;
}
declare namespace UpdateIdClaimHandler {
    const $inject: string[];
}
export default UpdateIdClaimHandler;
