-- Rancor Chain

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerHand, q.Other, o.Order, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerHand, q.Count, 2
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and not obj.Activated() and obj.CanSB(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		local t = false
		obj.Draw(2)
		if obj.Exists(2) then
			t = obj.YesNo("Discard an order card?")
			if t then
				obj.Discard(2)
			end
		end
		if not t then
			obj.Discard(3) 
		end
	elseif n == 2 then
		if obj.OrderPlayed() then
			obj.SetAbilityPower(4, 2000)
		else
			obj.SetAbilityPower(4, 0)
		end
	end
	return 0
end