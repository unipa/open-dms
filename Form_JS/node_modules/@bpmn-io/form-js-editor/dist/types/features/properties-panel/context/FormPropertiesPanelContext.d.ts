export default PropertiesPanelContext;
declare const PropertiesPanelContext: import("preact").Context<{
    getService: typeof getService;
}>;
/**
 * @param {string} type
 * @param {boolean} [strict]
 *
 * @returns {any}
 */
declare function getService(type: string, strict?: boolean): any;
