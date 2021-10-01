-- Joining Clasp, Ernesta

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.BackRow, q.Other, o.This, q.Other, o.Resting, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Applicable, q.Race, r.Ghost, q.Other, o.NotThis, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsPlayerTurn() and obj.IsBattlePhase() and obj.Exists(1) and obj.Exists(2) then
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
		obj.Stand(1)
	end
	return 0
end