-- Blaze Maiden, Reiyu

function NumberOfAbilities()
	return 2
end

function NumberOfParams(n)
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Deck, q.Name, "Vairina", q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Other, o.This
	end
end


function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, p.SB, 1
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Chakrabarthi Divine Dragon, Nirvana") then
			return true
		end
	elseif n == 2 then
		if obj.IsAttackingUnit() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Search(1)
		obj.Shuffle()
	elseif n == 2 then
		obj.AddBattleOnlyPower(2, 2000)
	end
	return 0
end