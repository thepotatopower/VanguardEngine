-- Sylvan Horned Beast, Bilber

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 6
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.Looking, q.Other, o.Unit, q.Count, 1, q.Min, 0
	elseif n == 4 then
		return q.Location, l.BackRow
	elseif n == 5 then
		return q.Location, l.Selected
	elseif n == 6 then
		return q.Location, l.Looking
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1, p.Retire, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.CanSB(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
		obj.Retire(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(3)
		obj.Select(3)
		obj.SuperiorCall(5, FL.BackRow)
		obj.DisableMove(5)
		obj.AddToDrop(6)
	end
	return 0
end