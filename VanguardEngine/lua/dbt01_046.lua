-- Pentagleam Sorceress

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.VanguardIs("Hexaorb Sorceress") then
			return true
		end
	elseif n == 2 then
		if obj.LastPlacedOnRC() then
			return true
		end
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(3)
		obj.RearrangeOnTop(1)
	elseif n == 2 then
		local t = 0
		obj.LookAtTopOfDeck(1)
		obj.DisplayCards(1)
		t = obj.SelectOption("Put on top of deck.", "Put on bottom of deck.")
		if t == 1 then
			obj.SendToTop(1)
		else
			obj.SendToBottom(1)
			obj.AddTempPower(2, 2000)
		end
	end
	return 0
end