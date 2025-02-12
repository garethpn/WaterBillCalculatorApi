import React, {useState, useEffect} from 'react';
import axiosInstance from '../api/axiosInstance';

const MeterDetails = () => {
    const [meters, setMeters] = useState([]);
    const [readings, setReadings] = useState({});
    const [previousReadings, setPreviousReadings] = useState({});
    const [billDetails, setBillDetails] = useState({
        standingCharge: '',
        unitPrice: '',
        billedUnits: '',
        billedAmount: '',
        meterReadings: []
    });
    const [billBreakdown, setBillBreakdown] = useState(null);

    useEffect(() => {
        axiosInstance.get('/WaterBill/MeterDetails')
            .then(response => {
                console.log(response.data);
                setMeters(response.data);
            })
            .catch(error => console.error('Error fetching meter details:', error));
    }, []);

    const handleReadingChange = (meterId, value) => {
        setReadings({...readings, [meterId]: value});
    };

    const handlePreviousReadingChange = (meterId, value) => {
        setPreviousReadings({...previousReadings, [meterId]: value});
    };

    const handleBillDetailChange = (e) => {
        const {name, value} = e.target;
        setBillDetails({
            ...billDetails,
            [name]: value
        });
    };

    const calculateBill = () => {
        const updatedBillDetails = {
            ...billDetails,
            meterReadings: Object.keys(readings).map(meterId => ({
                meterId: parseInt(meterId),
                reading: parseFloat(readings[meterId]),
                previousReading: previousReadings[meterId]
            }))
        };

        axiosInstance.post('/WaterBill/GetBillBreakdown', updatedBillDetails)
            .then(response => setBillBreakdown(response.data))
            .catch(error => console.error('Error calculating bill:', error));
    };

    return (
        <div className="container mt-5">
            <h1 className="mb-4">Bill Details</h1>
            <div className="mb-3 row g-3">
                <div className="col-1">
                    <label className="form-label">Bill Date:</label>
                </div>
                <div className="col-2">
                    <input
                        type="date"
                        className="form-control"
                        placeholder="Select date"
                        name="billDate"
                        value={billDetails.billDate}
                        onChange={handleBillDetailChange}
                    />
                </div>
            </div>
            <div className="mb-3">
                <label className="form-label">Standing Charge:</label>
                <input
                    type="number"
                    className="form-control"
                    name="standingCharge"
                    value={billDetails.standingCharge}
                    onChange={handleBillDetailChange}
                />
            </div>
            <div className="mb-3">
                <label className="form-label">Unit Price:</label>
                <input
                    type="number"
                    className="form-control"
                    name="unitPrice"
                    value={billDetails.unitPrice}
                    onChange={handleBillDetailChange}
                />
            </div>
            <div className="mb-3">
                <label className="form-label">Billed Units:</label>
                <input
                    type="number"
                    className="form-control"
                    name="billedUnits"
                    value={billDetails.billedUnits}
                    onChange={handleBillDetailChange}
                />
            </div>
            <div className="mb-3">
                <label className="form-label">Billed Amount:</label>
                <input
                    type="number"
                    className="form-control"
                    name="billedAmount"
                    value={billDetails.billedAmount}
                    onChange={handleBillDetailChange}
                />
            </div>
            <h1 className="mb-4">Meter Details</h1>
            {meters.map(meter => (
                <div key={meter.id} className="mb-3">
                    <h2>{meter.meterName}</h2>
                    <div className="mb-3">
                        <label className="form-label">Last Meter Reading:</label>
                        <input
                            type="number"
                            className="form-control"
                            placeholder="Enter reading"
                            value={meter.latestReading ?? previousReadings[meter.id] ?? ''}
                            readOnly={meter.latestReading !== null}
                            onChange={e => handlePreviousReadingChange(meter.id, e.target.value)}
                        />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">New Meter Reading:</label>
                        <input
                            type="number"
                            className="form-control"
                            placeholder="Enter reading"
                            onChange={e => handleReadingChange(meter.id, e.target.value)}
                        />
                    </div>
                </div>
            ))}
            <button className="btn btn-primary mt-3" onClick={calculateBill}>Calculate</button>
            {billBreakdown && (
                <div className="mt-5">
                    <h2>Bill Breakdown</h2>
                    <p>Standing Charge: {billBreakdown.standingCharge}</p>
                    <p>Total Bill Amount: {billBreakdown.totalBillAmount}</p>
                    <p>Remainder: {billBreakdown.remainder}</p>
                    <h3>Meter Shares</h3>
                    {billBreakdown.meterShares.map(share => (
                        <div key={share.meterId}>
                            <p>Meter {share.meterId}: {share.calculatedBillShare}</p>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default MeterDetails;