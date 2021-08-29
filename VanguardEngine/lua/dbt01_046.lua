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
		return a.OnRide, t.Auto, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Hexaorb Sorceress") then
			return true
		end
	elseif n == 2 then
		if obj.LastPlacedOnRC() then
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