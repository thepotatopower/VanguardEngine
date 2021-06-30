-- Vairina Arcs

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Trickstar"
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OverDress, 1
	elseif n == 2 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 2 then
		if obj.LastPlacedOnRC() and obj.InOverDress() and obj.CanCB(2) then
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
	if n == 2 then
		obj.Draw(2)
		obj.AddTempPower(3, 5000)
	end
	return 0
end
