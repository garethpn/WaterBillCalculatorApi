import React, {useState, useEffect} from 'react';
import axiosInstance from '../api/axiosInstance';
import 'bootstrap/dist/css/bootstrap.min.css';

const MeterDetails = () => {
    const [meters, setMeters] = useState([]);
    const [readings, setReadings] = useState({});
    const [previousReadings, setPreviousReadings] = useState({});
    const [billDetails, setBillDetails] = useState({
        billDate: new Date().toISOString().split('T')[0], // Default to current date
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
        <div className="parallax">
            <div className="container mt-5">
                <h1 className="page-title">Water Bill Calculator</h1>
                <div className="card mb-4">
                    <div className="card-header text-start">
                        <h4 className="mb-0">Bill Details</h4>
                    </div>
                    <div className="card-body">
                        <div className="mb-3 row g-3">
                            <div className="col-md-6">
                                <div className="input-group">
                                    <label className="input-group-text">Bill Date:</label>
                                    <input
                                        type="date"
                                        className="form-control"
                                        name="billDate"
                                        value={billDetails.billDate}
                                        onChange={handleBillDetailChange}
                                    />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="input-group">
                                    <label className="input-group-text">Standing Charge:</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        name="standingCharge"
                                        value={billDetails.standingCharge}
                                        onChange={handleBillDetailChange}
                                    />
                                </div>
                            </div>
                        </div>
                        <div className="mb-3 row g-3">
                            <div className="col-md-6">
                                <div className="input-group">
                                    <label className="input-group-text">Unit Price:</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        name="unitPrice"
                                        value={billDetails.unitPrice}
                                        onChange={handleBillDetailChange}
                                    />
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="input-group">
                                    <label className="input-group-text">Billed Units:</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        name="billedUnits"
                                        value={billDetails.billedUnits}
                                        onChange={handleBillDetailChange}
                                    />
                                </div>
                            </div>
                        </div>
                        <div className="mb-3 row g-3">
                            <div className="col-md-6">
                                <div className="input-group">
                                    <label className="input-group-text">Billed Amount:</label>
                                    <input
                                        type="number"
                                        className="form-control"
                                        name="billedAmount"
                                        value={billDetails.billedAmount}
                                        onChange={handleBillDetailChange}
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="card mb-4">
                    <div className="card-header text-start">
                        <h4 className="mb-0">Meter Readings</h4>
                    </div>
                    <div className="card-body">
                        <div className="accordion" id="meterAccordion">
                            {meters.map((meter, index) => (
                                <div key={meter.id} className="accordion-item">
                                    <h2 className="accordion-header" id={`heading${index}`}>
                                        <button className="accordion-button" type="button" aria-expanded="true">
                                            {meter.meterName}
                                        </button>
                                    </h2>
                                    <div id={`collapse${index}`} className="accordion-collapse collapse show"
                                         aria-labelledby={`heading${index}`}>
                                        <div className="accordion-body">
                                            <div className="mb-3 row g-3">
                                                <div className="col-md-6">
                                                    <div className="input-group">
                                                        <label className="input-group-text">Last Meter Reading:</label>
                                                        <input
                                                            type="number"
                                                            className="form-control"
                                                            value={meter.latestReading ?? previousReadings[meter.id] ?? ''}
                                                            readOnly={meter.latestReading !== null}
                                                            onChange={e => handlePreviousReadingChange(meter.id, e.target.value)}
                                                        />
                                                    </div>
                                                </div>
                                                <div className="col-md-6">
                                                    <div className="input-group">
                                                        <label className="input-group-text">New Meter Reading:</label>
                                                        <input
                                                            type="number"
                                                            className="form-control"
                                                            placeholder="Enter reading"
                                                            onChange={e => handleReadingChange(meter.id, e.target.value)}
                                                        />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
                {billBreakdown && (
                    <div className="card mt-5">
                        <div className="card-header text-start">
                            <h4 className="mb-0">Bill Breakdown</h4>
                        </div>
                        <div className="card-body">
                            <p>Standing Charge: {billBreakdown.standingCharge}</p>
                            <p>Total Bill Amount: {billBreakdown.totalBillAmount}</p>
                            <p>Remainder: {billBreakdown.remainder}</p>
                            <h5>Meter Shares</h5>
                            {billBreakdown.meterShares.map(share => (
                                <div key={share.meterId}>
                                    <p>Meter {share.meterId}: {share.calculatedBillShare}</p>
                                </div>
                            ))}
                        </div>
                    </div>
                )}
                <button className="btn btn-primary mt-3" onClick={calculateBill}>Calculate Bill Breakdown</button>
            </div>
        </div>
    );
};

export default MeterDetails;