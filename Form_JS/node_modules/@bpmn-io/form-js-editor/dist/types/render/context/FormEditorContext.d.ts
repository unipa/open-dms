export default FormEditorContext;
declare const FormEditorContext: import("preact").Context<{
    getService: typeof getService;
}>;
/**
 * @param {string} type
 * @param {boolean} [strict]
 *
 * @returns {any}
 */
declare function getService(type: string, strict?: boolean): any;
