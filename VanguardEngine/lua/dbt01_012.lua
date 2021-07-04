-- Stealth Dragon, Tensha Stead

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 6
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerVC
	elseif n == 5 then
		return q.Location, l.Deck, q.Name, "", q.Count, 1
	elseif n == 6 then
		return q.Location, l.Selected
	end
end                                     

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHits, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsBooster() and obj.CanCB(2) and obj.ExistsAmong(4, 5, q.Name) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(2)
		obj.Retire(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.Select(3)
		obj.Inject(5, q.Name, obj.GetSelectedName(6))
		obj.Search(5)
	end
	return 0
end