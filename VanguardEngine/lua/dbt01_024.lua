-- Spurring Maiden, Ellenia

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Grade, 2, q.Grade, 1, q.Grade, 0, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Selected, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRCFromHand, t.Auto, p.HasPrompt, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then 
		if obj.LastPlacedOnRCFromHand() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Select(1)
		obj.SuperiorCall(2)
		obj.AddTempPower(2, 5000)
	end
	return 0
end