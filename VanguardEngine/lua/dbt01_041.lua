-- Fighting Dragon, Goldog Dragon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerOrder, q.Other, o.SetOrder, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Other, o.This, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDiscard, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsPlayerTurn() and obj.LastDiscarded() and obj.Exists(1) then
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
		obj.SuperiorCall(2)
	end
	return 0
end