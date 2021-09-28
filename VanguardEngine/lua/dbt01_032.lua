-- Phantasma Magician, Curtis

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.FrontRow
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.OnACT, p.HasPrompt, p.CB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.VanguardIs("Master of Gravity, Baromagnes") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.SoulCount() >= 10 then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(2)
	elseif n == 2 then
		obj.AddTempPower(1, 5000)
	end
	return 0
end