-- Throbbing Search, Loronerol

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Deck, q.Location, l.PlayerHand, q.Grade, 1, q.Other, o.Song, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Deck, q.Other, o.Revealed, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerHand, q.Other, o.Revealed, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.Exists(1) then
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
		obj.ChooseReveal(1)
		if obj.Exists(2) then
			obj.PutIntoOrderZone(2)
			obj.Shuffle()
		elseif obj.Exists(3) and obj.PutIntoOrderZone(3) then
			obj.Draw(1)
		end
	end
	return 0
end