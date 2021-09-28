-- Diaglass Sorceress

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking, q.Other, o.Unit, q.Count, 2, q.Min, 0
	elseif n == 2 then
		return q.Location, l.Looking, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.CB, 1, p.Discard, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRCFromHand() and (obj.VanguardIs("Hexaorb Sorceress") or obj.VanguardIs("Pentagleam Sorceress")) then
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
		obj.LookAtTopOfDeck(2)
		obj.SuperiorCall(1)
		obj.RearrangeOnTop(2)
	end
	return 0
end