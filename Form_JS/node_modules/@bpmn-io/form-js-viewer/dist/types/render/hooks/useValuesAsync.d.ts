/**
 * @typedef {Object} ValuesGetter
 * @property {Object[]} values - The values data
 * @property {(LOAD_STATES)} state - The values data's loading state, to use for conditional rendering
 */
/**
 * A hook to load values for single and multiselect components.
 *
 * @param {Object} field - The form field to handle values for
 * @return {ValuesGetter} valuesGetter - A values getter object providing loading state and values
 */
export default function _default(field: any): ValuesGetter;
export type LOAD_STATES = string;
export namespace LOAD_STATES {
    const LOADING: string;
    const LOADED: string;
    const ERROR: string;
}
export type ValuesGetter = {
    /**
     * - The values data
     */
    values: any[];
    /**
     * - The values data's loading state, to use for conditional rendering
     */
    state: (LOAD_STATES);
};
