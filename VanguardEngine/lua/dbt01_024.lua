-- Spurring Maiden, Ellenia

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Drop, q.Grade, 2, q.Grade, 1, q.Grade, 0, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Selected, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRCFromHand, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then 
		if obj.LastPlacedOnRCFromHand() and obj.CanCB(1) and obj.CanSB(2) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
		obj.SoulBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.Select(3)
		obj.SuperiorCall(4)
		obj.AddTempPower(4, 5000)
	end
	return 0
end