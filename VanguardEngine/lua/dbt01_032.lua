-- Phantasma Magician, Curtis

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 10
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 2
	elseif n == 3 then
		return q.Location, l.FrontRow
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.VanguardIs("Master of Gravity, Baromagnes") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.Exists(1) and obj.CanCB(2) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.CounterBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(2)
	elseif n == 2 then
		obj.AddTempPower(3, 5000)
	end
	return 0
end