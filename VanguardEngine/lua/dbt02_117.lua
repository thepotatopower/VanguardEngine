-- Sylvan Horned Beast, Bilber

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Looking, q.Other, o.Unit, q.Count, 1, q.Min, 0
	elseif n == 3 then
		return q.Location, l.BackRow
	elseif n == 4 then
		return q.Location, l.Selected
	elseif n == 5 then
		return q.Location, l.Looking
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, p.HasPrompt, p.SB, 1, p.Retire, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then
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

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(3)
		obj.Select(2)
		obj.SuperiorCall(4, FL.BackRow)
		obj.DisableMove(4)
		obj.AddToDrop(5)
	end
	return 0
end