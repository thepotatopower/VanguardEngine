-- Midnight Lesson, Vannes

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking, q.UnitType, u.Trigger, q.Count, 2, q.Min, 0
	elseif n == 2 then
		return q.Location, l.Applicable, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.CB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(2) then
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
		obj.LookAtTopOfDeck(5)
		obj.SuperiorCall(1, FL.OpenCircle)
		obj.Shuffle()
	end
	return 0
end