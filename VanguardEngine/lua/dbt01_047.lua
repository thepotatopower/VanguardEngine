-- Divine Sister, Tartine

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This, q.Other, o.Standing
	elseif n == 2 then
		return q.Location, l.Looking, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Looking
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.SB, 1, p.Rest, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() then
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
	local t = 0
	if n == 1 then
		obj.LookAtTopOfDeck(2)
		obj.RearrangeOnTop(2)
		obj.RearrangeOnBottom(3)
	end
	return 0
end