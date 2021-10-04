-- Precise Word Sense, Flor

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Revealed, q.Grade, 2, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Revealed, q.Other, o.Unit, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Revealed
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRCFromHand, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) then
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
		obj.RevealFromDeck(1)
		if obj.Exists(3) and not obj.Exists(2) then
			obj.SuperiorCallToSpecificCircle(3, FL.OpenCircle)
		end
		obj.SendToBottom(4)
	end
	return 0
end