declare class FormFieldRegistry {
    constructor(eventBus: any);
    _eventBus: any;
    _formFields: {};
    _ids: any;
    _keys: any;
    add(formField: any): void;
    remove(formField: any): void;
    get(id: any): any;
    getAll(): any[];
    forEach(callback: any): void;
    clear(): void;
}
declare namespace FormFieldRegistry {
    const $inject: string[];
}
export default FormFieldRegistry;
