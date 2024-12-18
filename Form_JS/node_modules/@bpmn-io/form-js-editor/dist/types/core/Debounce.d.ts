/**
 * A factory to create a configurable debouncer.
 *
 * @param {number|boolean} [config=true]
 */
declare function DebounceFactory(config?: number | boolean): (fn: any) => any;
declare namespace DebounceFactory {
    const $inject: string[];
}
export default DebounceFactory;
