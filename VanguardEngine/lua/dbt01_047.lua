-- Divine Sister, Tartine

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This, q.Other, o.Standing, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Looking, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Looking
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.CanSB(1) and obj.Exists(2) then
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
		obj.Rest(2)
	end
end

function Activate(n)
	local t = 0
	if n == 1 then
		obj.LookAtTopOfDeck(2)
		obj.ChooseSendToTop(3)
		obj.SendToBottom(4)
	end
	return 0
end