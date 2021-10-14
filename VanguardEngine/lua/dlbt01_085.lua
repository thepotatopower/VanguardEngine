-- Proof of Effort, Wilista

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Deck, q.Other, o.Gem, q.Count, 1, q.Min, 0
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, p.HasPrompt, p.IsMandatory, p.AddToSoul, 1
	elseif n == 2 then
		return a.OnOrderPlayed, p.HasPrompt, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.Exists(2) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.PlayerPlayedOrder() then
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
		obj.Search(2)
	elseif n == 2 then
		obj.AddTempPower(3, 5000)
	end
	return 0
end